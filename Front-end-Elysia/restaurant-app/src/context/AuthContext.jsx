import { createContext, useEffect, useState, useContext } from "react";
import { storage } from "../utils/storage";

export const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [auth, setAuth] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const authData = storage.getAuth();

    if (authData) {
      setAuth(authData);
    }

    setLoading(false);
  }, []);

  const login = (authData) => {
    storage.saveAuth(authData);
    setAuth(authData);
  };

  const logout = () => {
    storage.clearAuth();
    setAuth(null);
  };

  const value = {
    auth,
    login,
    logout,
    loading,
    isAuthenticated: !!auth,
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  return useContext(AuthContext);
}