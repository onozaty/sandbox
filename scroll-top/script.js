const scrollButton = document.getElementById('scrollButton');

scrollButton.addEventListener('click', function() {
  window.scroll({top: 0, behavior: 'smooth'});
});

const judgeShowScrollButton = function() {
  if (window.scrollY > 0) {
    scrollButton.classList.add('active');
  } else {
    scrollButton.classList.remove('active');
  }
};

window.addEventListener('scroll', judgeShowScrollButton);
judgeShowScrollButton();
