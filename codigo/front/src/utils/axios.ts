import axios from "axios";

const http = axios.create({
   baseURL: "http://localhost:3000",
   headers: {
      Accept: "*/*",
      "Content-Type": "application/json",
   },
});

export { http };
