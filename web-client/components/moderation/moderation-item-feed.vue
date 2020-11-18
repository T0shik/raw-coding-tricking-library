<template>
  <v-list>
    <v-list-item :to="`/moderation/${modItem.id}`"
                 :key="`moderation-item-${modItem.type}-${modItem.id}`"
                 v-for="modItem in content">
      <v-list-item-avatar>
        <user-header :image-url="modItem.user.image"/>
      </v-list-item-avatar>
      <v-list-item-content>
        <v-list-item-title v-if="modItem.currentObject">{{ modItem.currentObject.name }}</v-list-item-title>
        <v-list-item-title v-else-if="modItem.targetObject">{{ modItem.targetObject.name }}</v-list-item-title>
        <v-list-item-subtitle v-if="modItem.reason">{{ modItem.reason }}</v-list-item-subtitle>
      </v-list-item-content>
      <v-list-item-content>
        <v-list-item-title>
          <span class="text-h6" :class="changeTypeColour(modItem)">{{ changeType(modItem) }}</span>
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
</template>

<script>
import UserHeader from "@/components/user-header";
import {feed} from "@/components/feed";
import {modItemRenderer, VERSION_STATE} from "@/components/moderation";
import {EVENTS} from "@/data/events";
import {onContentUpdate} from "@/components/_shared";

export default {
  name: "moderation-item-feed",
  components: {UserHeader},
  mixins: [feed(''), modItemRenderer, onContentUpdate],
  fetch() {
    return this.loadContent()
  },
  methods: {
    onContentUpdate() {
      return this.reloadContent()
    },
    getContentUrl() {
      return `/api/moderation-items${this.query}`
    },
    changeType(modItem) {
      return modItem.currentObject === null ? "NEW"
        : modItem.targetObject === null ? "DELETE"
          : modItem.targetObject.state === VERSION_STATE.LIVE ? "MIGRATION"
            : "CHANGE";
    },
    changeTypeColour(modItem) {
      return modItem.currentObject === null ? 'green--text'
        : modItem.targetObject === null ? 'red--text'
          : modItem.targetObject.state === VERSION_STATE.LIVE ? 'orange--text'
            : 'orange--text';
    }
  }
}
</script>

<style scoped>

</style>
