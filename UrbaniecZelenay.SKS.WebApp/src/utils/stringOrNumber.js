export function stringOrNumber(val) {
  if (!isNaN(val)) {
    return Number(val);
  }
  return val;
}
