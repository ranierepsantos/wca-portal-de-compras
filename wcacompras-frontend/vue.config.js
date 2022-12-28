const { defineConfig } = require('@vue/cli-service')
module.exports = defineConfig({
  transpileDependencies: true,
  devServer: {
    port: 2808,
    proxy: {
      '^/api': {
        //target: 'https://wca-backend.azurewebsites.net/',
        target: 'http://localhost:5031',
        changeOrigin: true,
        secure: true,
        logLevel: 'error'
      }
    }
  }
})
