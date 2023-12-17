FROM node:14-alpine

RUN adduser -D -H -u 10000 bojapp

#Fixed Error correctMkdir failed to make directory /home/bojapp/.npm/_locks
RUN mkdir -p /home/bojapp/app/node_modules && chown -R bojapp:bojapp /home/bojapp/app && mkdir -p /home/bojapp/.npm && chown -R bojapp:bojapp /home/bojapp/.npm 
WORKDIR /home/bojapp/app

USER bojapp
COPY --chown=bojapp:bojapp package*.json ./

RUN npm install
COPY --chown=bojapp:bojapp . .

EXPOSE 3000
# Override the default command for production mode.
CMD ["npm", "start"]