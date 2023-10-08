import React from "react";
import ReactDOM from "react-dom/client";
import "./index.scss";
import App from "./App";
import { ThemeProvider } from "@mui/material";
import { LightTheme } from "./theme/light.ts";
import { BrowserRouter } from "react-router-dom";
import { UserProvider } from "./contexts/user.context.jsx";
import { ProdutoProvider } from "./contexts/products.context";
import { FavoritoProvider } from "./contexts/favorito.contex";
import { CartProvider } from "./contexts/cart.context";
import { CategoriasProvider } from "./contexts/categorias.context";

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <React.StrictMode>
    <BrowserRouter>
      <CategoriasProvider>
        <ProdutoProvider>
          <UserProvider>
            <FavoritoProvider>
              <CartProvider>
                <ThemeProvider theme={LightTheme}>
                  <App />
                </ThemeProvider>
              </CartProvider>
            </FavoritoProvider>
          </UserProvider>
        </ProdutoProvider>
      </CategoriasProvider>
    </BrowserRouter>
  </React.StrictMode>
);
