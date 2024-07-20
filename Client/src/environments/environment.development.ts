export const environment = {
  production: false,
  auth0: {
    domain: 'car-game-dev.eu.auth0.com',
    clientId: 'dyCIUohOHUbngduL3wjIfdLCrqVfCgIT',
    authorizationParams: {
      audience: 'https://localhost:7235',
      redirect_uri: 'https://localhost:7235',
    },
    errorPath: '/callback',
  },
  api: {
    apiUrl: 'https://localhost:7235',
  },
};
