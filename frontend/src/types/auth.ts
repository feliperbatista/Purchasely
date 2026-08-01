export interface User {
  id: string;
  name: string;
  email: string;
  role: "Admin" | "Buyer" | "Manager" | "Requester";
}

export interface LoginRequest {
  email: string;
  password: string;
}