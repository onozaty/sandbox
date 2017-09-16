var gettingAllCommands = browser.commands.getAll();
gettingAllCommands.then((commands) => {
  for (let command of commands) {
    console.log(command);
  }
});

browser.commands.onCommand.addListener((command) => {
  console.log("onCommand event received for message: ", command);
});
