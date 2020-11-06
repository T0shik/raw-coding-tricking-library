<template>
  <div>
    <v-list>
      <v-list-item :to="`/moderation/${modItem.id}`" :key="modItem.id" v-for="modItem in content">
        <v-list-item-avatar>
          <user-header :image-url="modItem.user.image"/>
        </v-list-item-avatar>
        <v-list-item-content>
          <v-list-item-title>{{ modItem.targetObject.name }}</v-list-item-title>
          <v-list-item-subtitle v-if="modItem.reason">{{ modItem.reason }}</v-list-item-subtitle>
        </v-list-item-content>
        <v-list-item-content>
          <v-list-item-title>
            <span class="text-h6" :class="modItem.targetObject.version === 1 ? 'green--text' : 'orange--text'">
              {{ modItem.targetObject.version === 1 ? 'NEW' : 'CHANGE' }}</span>
          </v-list-item-title>
        </v-list-item-content>
        <v-list-item-content>
          <v-list-item-title>Last Updated</v-list-item-title>
          <v-list-item-subtitle>{{ modItem.updated }}</v-list-item-subtitle>
        </v-list-item-content>
        <v-list-item-content>
          <div class="d-flex justify-end">Review Status</div>
          <div class="d-flex justify-end" v-if="modItem.reviews.length > 0">
            <div v-for="review in modItem.reviews">
              <v-icon :color="reviewStatusColor(review)">
                {{ reviewStatusIcon(review) }}
              </v-icon>
            </div>
          </div>
          <div class="d-flex justify-end" v-else>
            Waiting
          </div>
        </v-list-item-content>
      </v-list-item>
      <v-list-item v-if="!finished" @click="loadContent">
        <v-list-item-content class="d-flex justify-center">Load More</v-list-item-content>
      </v-list-item>
    </v-list>
  </div>
</template>

<script>
import {feed} from "@/components/feed";
import {modItemRenderer} from "@/components/moderation";
import UserHeader from "@/components/user-header";

export default {
  components: {UserHeader},
  mixins: [feed(''), modItemRenderer],
  fetch() {
    return this.loadContent()
  },
  methods: {
    getContentUrl() {
      return `/api/moderation-items${this.query}`
    },
  }
}
</script>

<style scoped>

</style>
