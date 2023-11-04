import { type ReactElement } from "react";
import { Route, Routes } from "react-router-dom";

import { PrivateWrapper } from "./components";
import { Login } from "../Login";
import { Home } from "../Home";
import { Admin } from "../Admin";
import { SignUp } from "../Login/pages";

export const Router = (): ReactElement => {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/signup" element={<SignUp />} />
      <Route element={<PrivateWrapper />}>
        <Route index element={<Home />} />
      </Route>
      <Route path="/admin" element={<Admin />} />
    </Routes>
  );
};
