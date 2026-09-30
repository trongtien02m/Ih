(()=>{"use strict";
const c=document.querySelector("#game"),x=c.getContext("2d"),hpEl=document.querySelector("#hp"),info=document.querySelector("#info"),msg=document.querySelector("#msg"),stick=document.querySelector("#stick"),knob=document.querySelector("#knob"),skillBtn=document.querySelector("#skill");
let W,H,dpr,move={x:0,y:0},touchId=null,last=performance.now(),kills=0,core=0,phase="COMBAT",skillCd=0,attackCd=0,spawnCd=0,hitCount=0;
const P={x:0,y:0,r:16,hp:100,max:100,speed:220}, enemies=[],numbers=[],fx=[];
function resize(){dpr=Math.min(devicePixelRatio||1,2);W=innerWidth;H=innerHeight;c.width=W*dpr;c.height=H*dpr;x.setTransform(dpr,0,0,dpr,0,0);if(!P.x){P.x=W/2;P.y=H/2}}addEventListener("resize",resize);resize();
const rnd=(a,b)=>a+Math.random()*(b-a),dist=(a,b)=>Math.hypot(a.x-b.x,a.y-b.y);
function spawn(){let side=Math.floor(Math.random()*4),e={r:13,hp:90,max:90,speed:rnd(42,72),hit:0};if(side===0){e.x=rnd(0,W);e.y=-20}else if(side===1){e.x=W+20;e.y=rnd(0,H)}else if(side===2){e.x=rnd(0,W);e.y=H+20}else{e.x=-20;e.y=rnd(0,H)}enemies.push(e)}
function damage(e,d){e.hp-=d;hitCount++;numbers.push({x:e.x,y:e.y,t:0,v:Math.round(d)});fx.push({x:e.x,y:e.y,t:0});if(e.hp<=0){let i=enemies.indexOf(e);if(i>=0)enemies.splice(i,1);kills++;core++;}}
function basic(){let best=null,bd=190;for(const e of enemies){let d=dist(P,e);if(d<bd){bd=d;best=e}}if(best){damage(best,18);attackCd=.42}}
function skill(){if(skillCd>0||phase!=="COMBAT")return;skillCd=8;for(const e of [...enemies])if(dist(P,e)<180)damage(e,72);navigator.vibrate?.(25)}
skillBtn.addEventListener("pointerdown",e=>{e.preventDefault();skill()});
function stickMove(e){let r=stick.getBoundingClientRect(),cx=r.left+r.width/2,cy=r.top+r.height/2,dx=e.clientX-cx,dy=e.clientY-cy,m=Math.hypot(dx,dy),lim=48,s=Math.min(1,lim/(m||1));dx*=s;dy*=s;knob.style.transform=`translate(${dx}px,${dy}px)`;move.x=dx/lim;move.y=dy/lim}
stick.addEventListener("pointerdown",e=>{touchId=e.pointerId;stick.setPointerCapture(touchId);stickMove(e)});
stick.addEventListener("pointermove",e=>{if(e.pointerId===touchId)stickMove(e)});
function end(e){if(e.pointerId!==touchId)return;touchId=null;move.x=move.y=0;knob.style.transform=""}stick.addEventListener("pointerup",end);stick.addEventListener("pointercancel",end);
addEventListener("keydown",e=>{if(e.code==="Space")skill();});
function update(dt){
 if(phase!=="COMBAT")return;
 let kx=(keys["KeyD"]?1:0)-(keys["KeyA"]?1:0),ky=(keys["KeyS"]?1:0)-(keys["KeyW"]?1:0),mx=move.x+kx,my=move.y+ky,m=Math.hypot(mx,my);if(m>1){mx/=m;my/=m}
 P.x=Math.max(P.r,Math.min(W-P.r,P.x+mx*P.speed*dt));P.y=Math.max(P.r,Math.min(H-P.r,P.y+my*P.speed*dt));
 skillCd=Math.max(0,skillCd-dt);attackCd-=dt;spawnCd-=dt;if(spawnCd<=0&&enemies.length<45&&kills+enemies.length<60){spawn();spawnCd=.13}
 if(attackCd<=0)basic();
 for(const e of enemies){let dx=P.x-e.x,dy=P.y-e.y,d=Math.hypot(dx,dy)||1;if(d>P.r+e.r+3){e.x+=dx/d*e.speed*dt;e.y+=dy/d*e.speed*dt}else{e.hit-=dt;if(e.hit<=0){P.hp-=7;e.hit=.8;navigator.vibrate?.(8);if(P.hp<=0){P.hp=0;phase="DEFEAT";msg.textContent="DEFEAT\nTap to restart"}}}}
 for(const a of numbers)a.t+=dt;for(const a of fx)a.t+=dt;while(numbers[0]?.t>.7)numbers.shift();while(fx[0]?.t>.25)fx.shift();
 if(kills>=60){phase="VICTORY";msg.textContent="VICTORY\nTap to restart"}
}
const keys={};addEventListener("keydown",e=>keys[e.code]=true);addEventListener("keyup",e=>keys[e.code]=false);
addEventListener("pointerdown",e=>{if(phase!=="COMBAT"&&!stick.contains(e.target)&&e.target!==skillBtn){kills=core=hitCount=0;P.hp=P.max;enemies.length=numbers.length=fx.length=0;phase="COMBAT";msg.textContent=""}});
function draw(){
 x.clearRect(0,0,W,H);x.fillStyle="#172018";x.fillRect(0,0,W,H);
 x.strokeStyle="#263829";x.lineWidth=1;for(let i=0;i<W;i+=48){x.beginPath();x.moveTo(i,0);x.lineTo(i,H);x.stroke()}for(let i=0;i<H;i+=48){x.beginPath();x.moveTo(0,i);x.lineTo(W,i);x.stroke()}
 for(const e of enemies){x.fillStyle="#8d3434";x.beginPath();x.arc(e.x,e.y,e.r,0,7);x.fill();x.fillStyle="#222";x.fillRect(e.x-16,e.y-22,32,4);x.fillStyle="#6f6";x.fillRect(e.x-16,e.y-22,32*Math.max(0,e.hp/e.max),4)}
 for(const f of fx){x.strokeStyle=`rgba(255,220,90,${1-f.t/.25})`;x.lineWidth=4;x.beginPath();x.arc(f.x,f.y,10+f.t*80,0,7);x.stroke()}
 x.fillStyle="#4cb8ff";x.beginPath();x.arc(P.x,P.y,P.r,0,7);x.fill();x.strokeStyle="#fff";x.lineWidth=3;x.stroke();
 x.textAlign="center";x.font="bold 15px system-ui";for(const n of numbers){x.globalAlpha=1-n.t/.7;x.fillStyle="#fff";x.fillText(n.v,n.x,n.y-20-n.t*35)}x.globalAlpha=1;
 hpEl.style.width=(P.hp/P.max*100)+"%";info.textContent=`Kills ${kills}/60 · Hits ${hitCount} · Core ${core}`;skillBtn.textContent=skillCd>0?skillCd.toFixed(1):"SKILL";
}
function loop(t){let dt=Math.min(.033,(t-last)/1000);last=t;update(dt);draw();requestAnimationFrame(loop)}requestAnimationFrame(loop);
})();