/* global chrome:false */
(function (chrome) {
  var replacers = [];

  function walk (node) {
    // Source: http://is.gd/mwZp7E

    if (!node) return;

    var child, next;

    switch (node.nodeType) {
      case 1: // Element
      case 9: // Document
      case 11: // Document fragment
        child = node.firstChild;
        while (child) {
          next = child.nextSibling;
          walk(child);
          child = next;
        }
        break;

      case 3: // Text node
        handleText(node);
        break;
    }
  }

  function handleText (textNode) {
    var v = textNode.nodeValue;

    for (var i = 0; i < replacers.length; i++) {
      var replacer = replacers[i];
      v = v.replace(new RegExp(replacer.regex, 'gim'), replacer.text);
    }

    textNode.nodeValue = v;
  }

  chrome.runtime.onMessage.addListener(function (message) {
    replacers = message;
    walk(document.head);
    walk(document.body);
    setTimeout(function () {
      walk(document.head);
      walk(document.body);
    }, 5000);
  });

  chrome.runtime.connect();
})(chrome);
