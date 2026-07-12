export interface User {
  id: string;
  name: string;
  email: string;
  role: "Admin" | "Buyer" | "Approver" | "Requester";
}

export interface LoginRequest {
  email: string;
  password: string;
}