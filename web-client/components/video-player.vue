<template>
  <div class="video-container">
    <div class="play-button" :class="{'hide': playing}" @click="playing = !playing">
      <v-icon size="78">mdi-play</v-icon>
    </div>
    <video ref="video"
           muted loop
           :src="`http://localhost:5000/api/videos/${video.videoLink}`"
           :poster="`http://localhost:5000/api/videos/${video.thumbLink}`"
           preload="none"
    ></video>
  </div>
</template>

<script>
  //todo take care of the localhost address ^^^

  export default {
    name: "video-player",
    props: {
      video: {
        required: true,
        type: String,
      }
    },
    data: () => ({
      playing: false
    }),
    watch: {
      playing: function (v) {
        if (v) {
          this.$refs.video.play()
        } else {
          this.$refs.video.pause()
        }
      }
    }
  }
</script>

<style lang="scss" scoped>
  .video-container {
    display: flex;
    position: relative;
    width: 100%;

    border-top-left-radius: inherit;
    border-top-right-radius: inherit;

    .play-button {
      position: absolute;
      display: flex;
      justify-content: center;
      align-items: center;
      background-color: rgba(0, 0, 0, 0.36);
      width: 100%;
      height: 100%;
      z-index: 2;

      &.hide {
        opacity: 0;
      }
    }

    video {
      width: 100%;
      z-index: 1;

      border-top-left-radius: inherit;
      border-top-right-radius: inherit;
    }
  }
</style>
