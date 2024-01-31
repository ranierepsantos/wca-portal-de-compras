import axios from "axios";

const DEFAULTAPI = process.env.VUE_APP_SHARE_API_URL;  

const api =  axios.create({
  baseURL: DEFAULTAPI,
  paramsSerializer: {
    indexes: null // by default: false
  }
});

export default api;

