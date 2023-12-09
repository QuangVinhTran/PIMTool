import { Injectable } from "@angular/core";
import { faTimes, faCheckCircle, faInfoCircle, faExclamationCircle } from '@fortawesome/free-solid-svg-icons'; 
@Injectable({
    providedIn: 'root',
  })
export class ToastService {
    faTimes = faTimes;
    toast({ title = "", message = "", type = "info", duration = 3000 }) {
        const main = document.getElementById("toast");
        if (main) {
          const toast = document.createElement("div");
      
          // Auto remove toast
          const autoRemoveId = setTimeout(function () {
            main.removeChild(toast);
          }, duration + 1000);
      
          // Remove toast when clicked
          toast.onclick = (e) => {
            if(e) {
              if (e.target) {
                main.removeChild(toast);
                clearTimeout(autoRemoveId);
              }
            }
          };
          const icons = {
            success: faCheckCircle,
            info: faInfoCircle,
            warning: faExclamationCircle,
            error: faExclamationCircle
          };
          type objectKey = keyof typeof icons;
          const myKey = type as objectKey;
          const icon = icons[myKey];
          const delay = (duration / 1000).toFixed(2);
      
          toast.classList.add("toast", `toast--${type}`);
          toast.style.animation = `slideInLeft ease .3s, fadeOut linear 1s ${delay}s forwards`;
      
          toast.innerHTML = `
                            <div class="toast__icon">
                                <i class="${icon}"></i>
                            </div>
                            <div class="toast__body">
                                <h3 class="toast__title">${title}</h3>
                                <p class="toast__msg">${message}</p>
                            </div>
                            <div class="toast__close">
                                <i class="fas fa-times"></i>
                            </div>
                        `;
          main.appendChild(toast);
        }
      }
}