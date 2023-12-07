import {animate, style, transition, trigger} from "@angular/animations";

export const slideAnimation = [
  trigger('slideDown', [
    transition(':enter', [
      style({
        transform: 'translateY(-100%)',
        'transform-origin': 'top'
      }),
      animate('400ms ease-out',
        style({
          transform: 'translateY(0)'
        }))
    ])
  ]),
  trigger('slideUp', [
    transition(':leave', [
      style({
        transform: 'translateY(0)',
        'transform-origin': 'top'
      }),
      animate('400ms ease-in',
        style({
          transform: 'translateY(-100%)'
        }))
    ])
  ])
]
