const { defineConfig } = require('@vue/cli-service')
module.exports = defineConfig({
  transpileDependencies: true,
  devServer: {
    port: 2808,
    proxy: {
      '^/api': {
        target: 'https://wca-api-hml.azurewebsites.net/',
        //target: 'http://localhost:5031',
        changeOrigin: true,
        secure: true,
        logLevel: 'error'
      },
      '^/reembolsoapi': {
        target: 'https://wca-api-reembolso-hml.azurewebsites.net/api',
        //target: 'http://localhost:5100/api',
        pathRewrite: {'^/reembolsoapi': ''},
        changeOrigin: true,
        secure: false,
        logLevel: 'error'
      }
    }
  }
})
