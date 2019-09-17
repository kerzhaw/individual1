/* global chrome:false */
(function (chrome) {
  var cachedData = [{
    'twitter': 'donnyDbag',
    'withInitialledOrMiddleName': 'Donny D-Bag',
    'trumpTower': 'D-Bag Tower',
    'presidentTrump': 'President* D-Bag',
    'trump': 'D-Bag',
    'fullTitledName': 'President* Donny D-Bag'
  }];

  function init () {
    var xhr = new XMLHttpRequest();
    xhr.open('GET', 'https://dlructfdlsyzm.cloudfront.net/data.json', true);
    xhr.onreadystatechange = function () {
      if (xhr.readyState === 4) {
        var json = JSON.parse(xhr.responseText);
        cachedData = json.data;
        console.info('Donny data received');
      }
    };
    xhr.send();
  }

  function getRandomData () {
    var randomIndex = Math.floor(Math.random() * cachedData.length);
    var randomData = cachedData[randomIndex];
    return randomData;
  }

  function buildReplacers () {
    var data = getRandomData();

    var replacers = [{
      // twitter
      regex: 'realdonaldtrump',
      text: data.twitter
    },
    {
      // withInitialledOrMiddleName
      regex: 'donald\\s?j?(\\.|ohn)? trump',
      text: data.withInitialledOrMiddleName
    },
    {
      // trumpTower
      regex: 'trump tower',
      text: data.trumpTower
    },
    {
      // presidentTrump
      regex: 'president trump',
      text: data.presidentTrump
    },
    {
      // trump
      regex: 'trump(?!et|ed)',
      text: data.trump
    },
    {
      // fullTitledName
      regex: 'president ((donald)?\\s?j?(\\.|ohn)? )?trump',
      text: data.fullTitledName
    }
    ];

    return replacers;
  }

  chrome.runtime.onConnect.addListener(function (port) {
    var replacers = buildReplacers();
    chrome.tabs.sendMessage(port.sender.tab.id, replacers);
  });

  init();
})(chrome);
