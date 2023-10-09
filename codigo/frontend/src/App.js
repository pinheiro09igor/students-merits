import { Outlet } from "react-router-dom";
import AllRoutes from "./routers/AllRoutes.component";
import Container from "@mui/material/Container";
import Footer from "./components/footer/footer.component";
import { Box } from "@mui/material";

const App = () => {
  return (
    <Container maxWidth="xl">
      <div className="App">
        <AllRoutes />
      </div>
      <Outlet />
    </Container>
  );
};

export default App;
