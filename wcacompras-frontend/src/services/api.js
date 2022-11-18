import axios from "axios";

const DEFAULTAPI = process.env.VUE_APP_API_URL;  



const api =  axios.create({
  baseURL: DEFAULTAPI
});

api.interceptors.request.use(async config =>
{
    let token = localStorage.getItem("token");
    if (token != null) {
        config.headers["Authorization"] = `Bearer ${token}`;
    }
    return config;
});

export default api;

