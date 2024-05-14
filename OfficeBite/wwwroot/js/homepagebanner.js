window.addEventListener('load', () => {
    let deferredPrompt;

    const installBanner = document.getElementById('install-banner');
    const installButton = document.getElementById('install-button');
    const dismissButton = document.getElementById('dismiss-button');

    window.addEventListener('beforeinstallprompt', (e) => {
        e.preventDefault();
        deferredPrompt = e;
        installBanner.style.display = 'block';
    });

    installButton.addEventListener('click', () => {
     
        if (deferredPrompt) {
            
            deferredPrompt.prompt();
    
            deferredPrompt.userChoice.then((choiceResult) => {
             
                if (choiceResult.outcome === 'accepted') {
                    console.log('Потребителят е инсталирал приложението');
                }
      
                deferredPrompt = null;
           
                installBanner.style.display = 'none';
            });
        }
    });
   
    dismissButton.addEventListener('click', () => {
        installBanner.style.display = 'none';
    });
});
