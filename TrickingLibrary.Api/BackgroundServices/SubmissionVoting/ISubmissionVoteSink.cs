using System.Threading.Tasks;

namespace TrickingLibrary.Api.BackgroundServices.SubmissionVoting
{
    public interface ISubmissionVoteSink
    {
        ValueTask Submit(VoteForm voteForm);
    }
}