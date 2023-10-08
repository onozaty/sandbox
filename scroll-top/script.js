document.addEventListener('DOMContentLoaded', function() {

  const scrollTopButton = document.getElementById('scrollTopButton');

  scrollTopButton.addEventListener('click', function() {
    window.scroll({top: 0, behavior: 'smooth'});
  });
  
  const judgeShowScrollButton = function() {
    if (window.scrollY > 0) {
      scrollTopButton.classList.add('active');
    } else {
      scrollTopButton.classList.remove('active');
    }
  };
  
  window.addEventListener('scroll', judgeShowScrollButton);
  judgeShowScrollButton();
});
