export interface UserProfile {
    name: string;
    initials: string;
    email: string;
    role: string;
}

export interface LoginResponse {
    success: boolean;
    message: string;
    token: string;
    user: UserProfile;
}
