const endpointResolver = (type) => {
  if (type === MODERATION_TYPES.TRICK) return 'tricks'
  if (type === MODERATION_TYPES.CATEGORY) return 'categories'
  if (type === MODERATION_TYPES.DIFFICULTY) return 'difficulties'
}

export const MODERATION_TYPES = {
  TRICK: 'trick',
  CATEGORY: 'category',
  DIFFICULTY: 'difficulty',
}

export const REVIEW_STATUS = {
  APPROVED: 0,
  REJECTED: 1,
  WAITING: 2,
}
export const VERSION_STATE = {
  LIVE: 0,
  STAGED: 1,
  OUTDATED: 2,
}

const reviewStatusColor = (status) => {
  if (REVIEW_STATUS.APPROVED === status) return "green"
  if (REVIEW_STATUS.REJECTED === status) return "red"
  if (REVIEW_STATUS.WAITING === status) return "orange"
  return ''
}

const reviewStatusIcon = (status) => {
  if (REVIEW_STATUS.APPROVED === status) return "mdi-check"
  if (REVIEW_STATUS.REJECTED === status) return "mdi-close"
  if (REVIEW_STATUS.WAITING === status) return "mdi-clock"
  return ''
}

export const modItemRenderer = {
  methods: {
    endpointResolver,
    reviewStatusColor,
    reviewStatusIcon,
  }
}
