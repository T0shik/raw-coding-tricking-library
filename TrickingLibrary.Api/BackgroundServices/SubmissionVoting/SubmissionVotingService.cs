using System;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api.BackgroundServices.SubmissionVoting
{
    public class SubmissionVotingService : BackgroundService, ISubmissionVoteSink
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SubmissionVotingService> _logger;
        private readonly Channel<VoteForm> _channel;

        public SubmissionVotingService(
            IServiceProvider serviceProvider,
            ILogger<SubmissionVotingService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _channel = Channel.CreateUnbounded<VoteForm>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await _channel.Reader.WaitToReadAsync(stoppingToken))
            {
                var message = await _channel.Reader.ReadAsync(stoppingToken);
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var vote = ctx.SubmissionVotes
                        .FirstOrDefault(x => x.SubmissionId == message.SubmissionId
                                             && x.UserId == message.UserId);

                    if (vote == null)
                    {
                        ctx.Add(new SubmissionVote
                        {
                            SubmissionId = message.SubmissionId,
                            UserId = message.UserId,
                            Value = message.Value,
                        });
                    }
                    else
                    {
                        vote.Value = message.Value;
                    }

                    await ctx.SaveChangesAsync(stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Exception during submission vote processing.");
                }
            }
        }

        public ValueTask Submit(VoteForm voteForm)
        {
            return _channel.Writer.WriteAsync(voteForm);
        }
    }
}