document.addEventListener("keydown", (e) => {
  console.log(e);
});

setTimeout(() => {
  document.querySelector('body').focus();
}, 100);
