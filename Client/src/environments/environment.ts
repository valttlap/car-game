export const environment = {
  production: true,
  auth0: {
    domain: 'car-game.eu.auth0.com',
    clientId: '8OUHBfuayAu0ezDia1xChO5c7BeTJtiv',
    authorizationParams: {
      audience: 'https://valtsujaviltsu.fi',
      redirect_uri: 'https://valtsujaviltsu.fi',
    },
    errorPath: '/callback',
  },
  api: {
    apiUrl: 'https://valtsujaviltsu.fi',
  },
};
