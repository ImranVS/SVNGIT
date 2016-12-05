System.config({
    defaultJSExtensions: true,
    transpiler: 'typescript',
    typescriptOptions: {
        emitDecoratorMetadata: true
    },
    paths: {
        //'npm:': 'https://unpkg.com/',
        'npm:': 'angular2/',
        'dragula': 'lib/dragula/dragula.js',
        'ng2-dragula': 'lib/ng2-dragula/',
        'angular2-jwt': 'lib/angular2-jwt/angular2-jwt.js'
    },
    map: {

        'app': 'app',

        '@angular/core': 'npm:@angular/core/bundles/core.umd.js',
        '@angular/common': 'npm:@angular/common/bundles/common.umd.js',
        '@angular/compiler': 'npm:@angular/compiler/bundles/compiler.umd.js',
        '@angular/platform-browser': 'npm:@angular/platform-browser/bundles/platform-browser.umd.js',
        '@angular/platform-browser-dynamic': 'npm:@angular/platform-browser-dynamic/bundles/platform-browser-dynamic.umd.js',
        '@angular/http': 'npm:@angular/http/bundles/http.umd.js',
        '@angular/router': 'npm:@angular/router/bundles/router.umd.js',
        '@angular/forms': 'npm:@angular/forms/bundles/forms.umd.js',

        'rxjs': 'npm:rxjs',

        'ng2-dragula': 'ng2-dragula',

        'angular-jwt': 'angular2-jwt'
    },
    packages: {
        app: {
            main: 'main.js',
            defaultExtension: 'js',
            format: 'register'
        },
        rxjs: {
            defaultExtension: 'js'
        }
    }
});
