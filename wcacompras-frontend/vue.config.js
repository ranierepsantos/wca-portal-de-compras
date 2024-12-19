const { defineConfig } = require('@vue/cli-service')
module.exports = defineConfig({
  chainWebpack: (config) => {
    config.plugin('define').tap((definitions) => {
      Object.assign(definitions[0], {
        __VUE_OPTIONS_API__: 'true',
        __VUE_PROD_DEVTOOLS__: 'false',
        __VUE_PROD_HYDRATION_MISMATCH_DETAILS__: 'false'
      })
      return definitions
    })
  },
  transpileDependencies: true,
  devServer: {
    port: 2808,
    proxy: {
      '^/api': {
        //target: 'https://wca-api-hml.azurewebsites.net/',
        target: 'http://localhost:5031',
        changeOrigin: true,
        secure: true,
        logLevel: 'error'
      },
      '^/reembolsoapi': {
        target: 'https://wca-api-reembolso-hml.azurewebsites.net/api',
        target: 'http://localhost:5100/api',
        pathRewrite: {'^/reembolsoapi': ''},
        changeOrigin: true,
        secure: false,
        logLevel: 'error'
      },
      '^/shareapi': {
        //target: 'https://wca-api-share-hml.azurewebsites.net/api',
        target: 'http://localhost:5030/api',
        pathRewrite: {'^/shareapi': ''},
        changeOrigin: true,
        secure: false,
        logLevel: 'error'
      }
    }
  }
})
