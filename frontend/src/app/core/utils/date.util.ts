function formatDateTime(dateTime: string): string {
  const dateFormat = 'yyyy-mm-dd'
  return dateTime.slice(0, dateFormat.length)
}

export {
  formatDateTime
}
