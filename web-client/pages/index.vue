<template>
  <div>
    <div>
      <h3 class="text-h3 text-center">Tricks</h3>
      <front-page-trick-feed/>
    </div>
    <v-divider class="my-5"></v-divider>
    <div>
      <h3 class="text-h3 text-center">Categories</h3>
      <front-page-category-feed/>
    </div>
    <v-divider class="my-5"></v-divider>
    <div>
      <h3 class="text-h3 text-center">Difficulties</h3>
      <front-page-difficulty-feed/>
    </div>
  </div>
</template>

<script>
import FrontPageTrickFeed from "@/components/front-page/front-page-trick-feed";
import FrontPageCategoryFeed from "@/components/front-page/front-page-category-feed";
import FrontPageDifficultyFeed from "@/components/front-page/front-page-difficulty-feed";

export default {
  components: {FrontPageDifficultyFeed, FrontPageCategoryFeed, FrontPageTrickFeed},
}
</script>
