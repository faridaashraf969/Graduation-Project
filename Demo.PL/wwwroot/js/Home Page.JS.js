// Function to toggle the navigation menu
function toggleMenu() {
    var nav = document.getElementById("nav");
    nav.classList.toggle("active");
  }
  
  // Function to handle the click event on the navigation button
  function handleClick() {
    alert("You clicked the button!");
  }
  
  // Add event listeners to the navigation button and menu
  var navButton = document.getElementById("navButton");
  navButton.addEventListener("click", handleClick);
  
  var navMenu = document.getElementById("navMenu");
  navMenu.addEventListener("click", toggleMenu);