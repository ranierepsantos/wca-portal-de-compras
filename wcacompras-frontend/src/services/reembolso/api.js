import axios from "axios";

const DEFAULTAPI = process.env.VUE_APP_REEMBOLSO_API_URL;  

const api =  axios.create({
  baseURL: DEFAULTAPI
});

export default api;

