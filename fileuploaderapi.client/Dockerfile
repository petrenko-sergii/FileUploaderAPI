FROM node:20-alpine AS frontend

WORKDIR /app

COPY . .
RUN npm install
RUN npm run build

FROM httpd:alpine

WORKDIR /usr/local/apache2/htdocs
COPY --from=frontend /app/dist/fileuploaderapi.client .
