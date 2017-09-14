async function startup(tab) {

  await browser.tabs.executeScript(tab.id, {
    file: "/startup.js"
  });
}


browser.tabs.onUpdated.addListener((id, changeInfo, tab) => {
  if (tab.status === 'complete') {
    startup(tab);
  }
});
