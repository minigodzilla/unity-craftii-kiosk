mergeInto(LibraryManager.library, {
    SendMessageToJS: function (message) {
        let msg = UTF8ToString(message);
        console.log('Received message from Unity:', msg);

        if (!window.unityInstance) {
            console.error('Unity instance not available. Cannot send messages.');
            return;
        }

        if (msg === 'RequestURLParams') {
            const params = new URLSearchParams(window.location.search);
            if (params.has('c')) {
                let configCode = params.get('c');
                if (/^\d{4}$/.test(configCode)) {
                    console.log('Sending config to Unity:', configCode);
                    window.unityInstance.SendMessage('ObjectController', 'SetConfigFromJS', configCode);
                } else {
                    console.error('Invalid config format:', configCode);
                }
            }
        }
    },
});
