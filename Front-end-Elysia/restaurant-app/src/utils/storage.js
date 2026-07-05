const AUTH_KEY = "elysia_auth";

export const storage = {
  saveAuth(authData) {
    localStorage.setItem(AUTH_KEY, JSON.stringify(authData));
  },

  getAuth() {
    const auth = localStorage.getItem(AUTH_KEY);
    return auth ? JSON.parse(auth) : null;
  },

  clearAuth() {
    localStorage.removeItem(AUTH_KEY);
  },
};