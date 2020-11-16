const endpointResolver = (type) => {
  if (type === 'trick') return 'tricks'
  if (type === 'category') return 'categories'
}

export const REVIEW_STATUS = {
  APPROVED: 0,
  REJECTED: 1,
  WAITING: 2,
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
