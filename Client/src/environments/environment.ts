export const environment = {
  production: true,
  auth0: {
    domain: 'car-game.eu.auth0.com',
    clientId: '8OUHBfuayAu0ezDia1xChO5c7BeTJtiv',
    authorizationParams: {
      audience: 'https://car-api.valtterilappalainen.dev/',
      redirect_uri: 'https://viltsujavaltsu.fi',
    },
    errorPath: '/callback',
  },
  api: {
    apiUrl: 'https://viltsujavaltsu.fi',
  },
};
