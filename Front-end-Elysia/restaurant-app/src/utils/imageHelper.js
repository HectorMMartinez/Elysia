const API_BASE_URL =
  import.meta.env.VITE_API_URL.replace("/Api/v1", "") || "https://localhost:7108";

export function getProductImageUrl(image) {
  if (!image) return "";

  if (
    image.startsWith("http://") ||
    image.startsWith("https://") ||
    image.startsWith("blob:") ||
    image.startsWith("data:")
  ) {
    return image;
  }

  const normalizedPath = image.startsWith("/") ? image : `/${image}`;

  return `${API_BASE_URL}${normalizedPath}`;
}