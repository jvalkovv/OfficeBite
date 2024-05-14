const CACHE_NAME = 'officebite-cache-v1';

//const urlsToCache = [
//    '/',
//    '/images/icons/logo192.png',
//    '/images/icons/logo512.png'
//];


//self.addEventListener('install', event => {
//    event.waitUntil(
//        caches.open(CACHE_NAME)
//            .then(cache => cache.addAll(urlsToCache))
//            .then(() => {
//                console.log('Cache successfully installed');
//            })
//    );
//});

//self.addEventListener('activate', event => {
//    event.waitUntil(
//        caches.keys().then(cacheNames => {
//            return Promise.all(
//                cacheNames.map(cacheName => {
//                    if (cacheName !== CACHE_NAME) {
//                        return caches.delete(cacheName);
//                    }
//                })
//            );
//        })
//            .then(() => {
//                console.log('Cache successfully activated');
//            })

//    );
//});



self.addEventListener('fetch', event => {
    event.respondWith(
        caches.match(event.request)
            .then(response => {

                if (response) {
                    return response;
                }

                return fetch(event.request);
            })
    );
});


if ('serviceWorker' in navigator) {
    window.addEventListener('load', () => {
        navigator.serviceWorker.register('/service-worker.js')
            .then(registration => {
                console.log('Service Worker registered:', registration);
            })
            .catch(error => {
                console.error('Service Worker registration failed:', error);
            });
    });
}

