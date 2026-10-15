---
name: Peyvand Support System
colors:
  surface: '#faf8ff'
  surface-dim: '#d2d9f4'
  surface-bright: '#faf8ff'
  surface-container-lowest: '#ffffff'
  surface-container-low: '#f2f3ff'
  surface-container: '#eaedff'
  surface-container-high: '#e2e7ff'
  surface-container-highest: '#dae2fd'
  on-surface: '#131b2e'
  on-surface-variant: '#464555'
  inverse-surface: '#283044'
  inverse-on-surface: '#eef0ff'
  outline: '#777587'
  outline-variant: '#c7c4d8'
  surface-tint: '#4d44e3'
  primary: '#3525cd'
  on-primary: '#ffffff'
  primary-container: '#4f46e5'
  on-primary-container: '#dad7ff'
  inverse-primary: '#c3c0ff'
  secondary: '#006c49'
  on-secondary: '#ffffff'
  secondary-container: '#6cf8bb'
  on-secondary-container: '#00714d'
  tertiary: '#684000'
  on-tertiary: '#ffffff'
  tertiary-container: '#885500'
  on-tertiary-container: '#ffd4a4'
  error: '#ba1a1a'
  on-error: '#ffffff'
  error-container: '#ffdad6'
  on-error-container: '#93000a'
  primary-fixed: '#e2dfff'
  primary-fixed-dim: '#c3c0ff'
  on-primary-fixed: '#0f0069'
  on-primary-fixed-variant: '#3323cc'
  secondary-fixed: '#6ffbbe'
  secondary-fixed-dim: '#4edea3'
  on-secondary-fixed: '#002113'
  on-secondary-fixed-variant: '#005236'
  tertiary-fixed: '#ffddb8'
  tertiary-fixed-dim: '#ffb95f'
  on-tertiary-fixed: '#2a1700'
  on-tertiary-fixed-variant: '#653e00'
  background: '#faf8ff'
  on-background: '#131b2e'
  surface-variant: '#dae2fd'
typography:
  headline-lg:
    fontFamily: Vazirmatn, Tahoma, sans-serif
    fontSize: 30px
    fontWeight: '700'
    lineHeight: 42px
  headline-lg-mobile:
    fontFamily: Vazirmatn, Tahoma, sans-serif
    fontSize: 24px
    fontWeight: '700'
    lineHeight: 34px
  headline-md:
    fontFamily: Vazirmatn, Tahoma, sans-serif
    fontSize: 22px
    fontWeight: '700'
    lineHeight: 32px
  headline-sm:
    fontFamily: Vazirmatn, Tahoma, sans-serif
    fontSize: 18px
    fontWeight: '600'
    lineHeight: 28px
  body-lg:
    fontFamily: Vazirmatn, Tahoma, sans-serif
    fontSize: 16px
    fontWeight: '400'
    lineHeight: 28px
  body-md:
    fontFamily: Vazirmatn, Tahoma, sans-serif
    fontSize: 14px
    fontWeight: '400'
    lineHeight: 24px
  body-sm:
    fontFamily: Vazirmatn, Tahoma, sans-serif
    fontSize: 12px
    fontWeight: '400'
    lineHeight: 20px
  label-lg:
    fontFamily: Vazirmatn, Tahoma, sans-serif
    fontSize: 14px
    fontWeight: '600'
    lineHeight: 20px
  label-md:
    fontFamily: Vazirmatn, Tahoma, sans-serif
    fontSize: 12px
    fontWeight: '500'
    lineHeight: 18px
  label-sm:
    fontFamily: Vazirmatn, Tahoma, sans-serif
    fontSize: 11px
    fontWeight: '500'
    lineHeight: 16px
rounded:
  sm: 0.25rem
  DEFAULT: 0.5rem
  md: 0.75rem
  lg: 1rem
  xl: 1.5rem
  full: 9999px
spacing:
  gutter: 1rem
  gutter-tablet: 1.25rem
  gutter-desktop: 1.5rem
  margin: 1rem
  margin-tablet: 1.5rem
  margin-desktop: 2rem
  space-xs: 0.25rem
  space-sm: 0.5rem
  space-md: 0.75rem
  space-lg: 1rem
  space-xl: 1.5rem
  space-2xl: 2rem
---

## Brand & Style

This design system establishes a high-clarity, trust-centered digital service environment optimized for modern Persian (RTL) enterprise and consumer support workflows. The emotional tone is authoritative yet empathetic, dependable, and swift. By prioritizing structured legibility and frictionless communication over superficial ornament, the interface instills confidence during critical service interactions.

The design movement combines **Modern Utility** with **Tactile Softness**:
- Structural discipline driven by crisp 1px borders and subtle tonal separation.
- Bi-directional visual hierarchy tuned for right-to-left reading paths, placing critical contextual anchors (ticket status, agent avatars, key actions) on the visual origin (right side) while maintaining secondary metrics and timestamps on the left.
- Soft, rounded forms and low-contrast elevation to soften the operational intensity of support management.

## Colors

The palette balances authoritative deep royal blue tones with unambiguous operational status signifiers.

### Role Mapping
- **Primary (`#4F46E5`, `#3730A3`)**: Designates primary actions, active navigation states, user message bubbles, and high-priority interactive touchpoints.
- **Secondary / Success (`#10B981`)**: Dedicated to resolved tickets, operational confirmations, online indicators, and positive resolution states.
- **Tertiary / Warning (`#F59E0B`)**: Denotes pending customer action, awaiting agent response, escalations, and SLA countdown warnings.
- **Neutral (`#0F172A`)**: Primary high-contrast text color on neutral slate backgrounds, preventing visual fatigue compared to pure black.
- **Surfaces & Borders**:
  - Base background: `#F8FAFC` (Slate 50)
  - Card & Container surface: `#FFFFFF`
  - Subtle borders: `#E2E8F0` (Slate 200)
  - Muted secondary copy: `#64748B` (Slate 500)

## Typography

Typography relies on a clean, modern Persian-first sans-serif stack (`Vazirmatn, Tahoma, sans-serif`) natively configured for `direction: rtl`. 

- **Line Height Adaptations**: Persian glyphs possess deeper ascenders and descenders compared to Latin scripts. The typography scale utilizes generous line-height ratios (1.5× to 1.75×) to eliminate visual overlap and preserve legibility across long chat transcripts and ticket descriptions.
- **Numerals**: All ticket IDs, timestamps, and metrics should support localized Persian numerals (`۱۲۳۴۵۶۷۸۹۰`) via font features (`ss01` or standard locale formatting) while retaining monospaced tabular alignment in data tables.
- **Hierarchy Rationale**: Headlines establish clear section demarcation without overpowering chat density; body styles are calibrated specifically for scanning conversational fragments.

## Layout & Spacing

The layout employs a mobile-first fluid model governed by a structured 8pt/4pt linear rhythm.

### Grid & Breakpoints
- **Mobile (< 640px)**: 4-column layout with 16px (`1rem`) outer margins and gutters. Single-column stacked flows where ticket conversations occupy 100% viewport width with sticky bottom action controls.
- **Tablet (640px – 1024px)**: 8-column layout with 20px (`1.25rem`) gutters. Collapsible list/detail view split horizontally or sliding side drawer for metadata.
- **Desktop (> 1024px)**: 12-column layout with 24px (`1.5rem`) gutters and a max canvas width of `1280px`. Left-pane context inspector (25%), center conversation panel (50%), and right-hand ticket navigator (25%) organized naturally for RTL visual priority.

### RTL Flow Rules
All spacing utilities mirror symmetrically: `margin-inline-start` anchors to the right, and `margin-inline-end` anchors to the left. Touch targets on mobile never drop below 44px × 44px.

## Elevation & Depth

Visual hierarchy leverages a hybrid model of **Tonal Layering** paired with **Ambient Diffused Shadows**, avoiding harsh drops to keep ticket interfaces calm.

- **Level 0 (Canvas Base)**: `#F8FAFC` flat surface.
- **Level 1 (Cards, Ticket Items, Form Rows)**: `#FFFFFF` surface bounded by a crisp `1px solid #E2E8F0` border. Ambient shadow: `0px 1px 3px rgba(15, 23, 42, 0.04), 0px 1px 2px rgba(15, 23, 42, 0.02)`.
- **Level 2 (Active Message Bubble, Floating Filter Bars, Dropdowns)**: Elevated surface with `0px 4px 12px -2px rgba(15, 23, 42, 0.08), 0px 2px 6px -1px rgba(15, 23, 42, 0.04)`.
- **Level 3 (Modals, Sticky Action Drawers, Priority Alerts)**: High elevation utilizing `0px 12px 24px -4px rgba(15, 23, 42, 0.12), 0px 4px 8px -2px rgba(15, 23, 42, 0.04)`.
- **Glass Accents**: Sticky navigation bars and floating compose inputs on mobile use `rgba(255, 255, 255, 0.88)` with `backdrop-filter: blur(8px)` and a bottom border of `#E2E8F0`.

## Shapes

The shape system is set to **Level 2 (Rounded)**, providing balanced corners (8px base, 16px for larger cards) that soften operational rigor without sacrificing enterprise density.

- **Inputs, Buttons, Badges**: `rounded-md` (`8px`) for high functional recognition and crisp hit areas.
- **Tickets, Informational Cards, Modals**: `rounded-lg` (`16px`) to frame discrete blocks of content clearly.
- **Chat Bubbles**: Asymmetric radii tailored to conversation direction:
  - *Customer message (Self / RTL start)*: Rounded on all sides (`16px`) with the bottom-right corner sharpened to `4px` pointing towards the sender.
  - *Agent / System response*: Rounded on all sides (`16px`) with the bottom-left corner sharpened to `4px`.
- **Status Pills & Avatars**: Full round (`9999px`) to maintain clear visual contrast against rectangular ticket cards.

## Components

### Buttons
- **Primary**: Deep Indigo background (`#4F46E5`), white text, `8px` corner radius, `12px 20px` padding. Hover: `#3730A3`. Active: Scale `0.98`.
- **Secondary / Ghost**: Transparent fill, `#E2E8F0` border, `#0F172A` text. Hover: `#F1F5F9`.
- **Destructive**: `#FEF2F2` background, `#DC2626` text and border for ticket closing or escalation flags.

### Status Badges & Chips
- Compact pills (`border-radius: 9999px`) with padding `4px 10px` containing small indicator dots (`6px` circular).
- **Open / Resolved**: `#ECFDF5` background, `#10B981` text, `#A7F3D0` subtle border.
- **In-Progress / Pending**: `#FFFBEB` background, `#F59E0B` text, `#FDE68A` subtle border.
- **Closed / Inactive**: `#F1F5F9` background, `#64748B` text, `#CBD5E1` subtle border.

### Chat Bubbles
- **Outbound (User/RTL right-aligned)**: `#4F46E5` background, `#FFFFFF` text, asymmetric corners (`16px 16px 4px 16px`). Metas (status checks, timestamps) render in `#C7D2FE` aligned to the left of the bubble.
- **Inbound (Agent/RTL left-aligned)**: `#FFFFFF` background with `1px solid #E2E8F0`, `#0F172A` text, asymmetric corners (`16px 16px 16px 4px`). Agent avatar positioned immediately outside the bubble edge.

### Input Fields & Textareas
- Base surface `#FFFFFF` with `#E2E8F0` stroke. On focus: `#4F46E5` border with an outer ring `0 0 0 3px rgba(79, 70, 229, 0.12)`.
- RTL placeholder alignment: Right-aligned text with optional accessory actions (attachments, voice notes) anchored to the left inline boundary.

### Cards & Ticket List Items
- Container features white background, `16px` radius, and 1px border `#E2E8F0`.
- Includes a 4px vertical accent bar along the right boundary (`border-inline-start`) indicating priority level (e.g., Red for critical, Amber for normal).
- Top row displays Ticket ID and Status Badge; middle row features 2-line truncated subject; bottom row holds agent avatar and relative Persian timestamp.