# PIMTool

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 8.0.1.

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `--prod` flag for a production build.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).

## Modules
#### 1. Base
Containt all common components, services, directives, helpers,... that used in project.
#### 2. Shell
Containt the master layout of the application.
#### 3. Project
The example module for the application, it containt the example with create grid component in Base module and use it in project list component.
## Multilingual
The project have aldready setup the multilingual with Angular TranslateModule, you can find the resource file in src/assets/i18n
## Swagger
* When you change the controller, you need to run "npm run generate-swagger" to generate typescript file to call to api, the output is located in src/app
* Note: if you want to use swagger api in your module, you have to import the ApiModule of swagger to your module
* When you use swagger to call to backend api, please check the url is correct, if not, please provide ApiConfiguration in your module (like in app.module.ts)
