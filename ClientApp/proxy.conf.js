const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:59528';

const PROXY_CONFIG = [
  {
    context: [
      "/api3/", "/api2/","apiremote/weatherforecast","/api/","/weatherforecast", "/bff", "/signin-oidc", "/signout-callback-oidc"
   ],
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  }
]

module.exports = PROXY_CONFIG;
