import axios from "axios";

const statusMessages: Record<number, string> = {
  400: "Invalid request",
  401: "Unauthorized",
  403: "You don't have permission to do this",
  404: "Not found",
  409: "A record with this information already exists",
  500: "Server error, please try again later",
};

export const getErrorMessage = (error: unknown): string => {
  if (axios.isAxiosError(error)) {
    const data = error.response?.data;
    const status = error.response?.status;

    if (Array.isArray(data) && data.length > 0)
      return data[0];

    if (data?.detail)
      return data.detail;

    if (data?.message)
      return data.message;

    if (Array.isArray(data?.errors) && data.errors.length > 0)
      return data.errors[0];

    if (status && statusMessages[status])
      return statusMessages[status];

    return error.message ?? "Something went wrong";
  }

  return "Something went wrong";
};