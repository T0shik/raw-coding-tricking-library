export const hasOccurrences = (searchIndex, query) => {
  const queryParts = query.toLowerCase().split(' ');
  if (queryParts.length > 0) {
    return queryParts.map(x => searchIndex.indexOf(x) > -1).reduce((a, b) => a && b)
  }

  return true;
}
