import Home from "./components/Home";
import  Stats from "./components/Stats";
import  SportObject from "./components/SportObject";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/stats',
    element: <Stats />
  },
  {
    path: '/object/:id',
    element: <SportObject />
  }
];

export default AppRoutes;
