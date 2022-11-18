const { defineConfig } = require('@vue/cli-service')
module.exports = defineConfig({
  transpileDependencies: true,
  devServer: {
    port: 2808,
    proxy: {
      '^/api': {
        target: 'http://localhost:5031/',
        //target:  'http://smb.cliqx.com.br:27118/',
        changeOrigin: true,
        secure: true,
        logLevel: 'error'
      }
    }
  }
})
