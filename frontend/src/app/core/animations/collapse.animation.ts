import {animate, style, transition, trigger} from "@angular/animations";

export const collapseAnimation = [
  trigger('open', [
    transition(':enter', [
      style({
        height: '0',
        transform: 'rotateX(90deg)',
        'transform-origin': 'top'
      }),
      animate('350ms ease',
        style({
          height: '*',
          transform: 'rotateX(0deg)'
        }))
    ])
  ]),
  trigger('close', [
    transition(':leave', [
      style({
        height: '*',
        transform: 'rotateX(0deg)',
        'transform-origin': 'top'
      }),
      animate('350ms ease',
        style({
          height: '0',
          transform: 'rotateX(90deg)'
        }))
    ])
  ])
]
