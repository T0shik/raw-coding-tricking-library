export const feed = (order) => ({
  props: {
    contentEndpoint: {
      required: false,
      type: String,
    }
  },
  data: () => ({
    content: [],
    cursor: 0,
    limit: 10,
    order: order,
    started: false,
    finished: false,
    loading: false,
  }),
  watch: {
    'order': function () {
      this.content = []
      this.cursor = 0
      this.finished = false
      this.started = false
      this.loadContent()
    }
  },
  methods: {
    getContentUrl() {
      return `${this.contentEndpoint}${this.query}`
    },
    onScroll() {
      if (this.finished || this.loading) return;
      const loadMore = document.body.offsetHeight - (window.pageYOffset + window.innerHeight) < 500

      if (loadMore) {
        this.loadContent()
      }
    },
    loadContent() {
      this.started = true
      this.loading = true
      return this.$axios.$get(this.getContentUrl())
        .then(content => {
          this.finished = content.length < this.limit
          this.parseContent(content)
          this.cursor += content.length;
        })
        .finally(() => this.loading = false)
    },
    parseContent(content) {
      content.forEach(x => this.content.push(x))
    }
  },
  computed: {
    query() {
      return `?order=${this.order}&cursor=${this.cursor}&limit=${this.limit}`
    }
  }
})
