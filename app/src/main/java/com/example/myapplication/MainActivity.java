package com.example.myapplication;


import android.content.Intent;
import android.os.Bundle;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ViewFlipper;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentTransaction;

import com.google.android.material.bottomnavigation.BottomNavigationView;

//public class MainActivity extends AppCompatActivity implements View.OnClickListener {
public class MainActivity extends AppCompatActivity  {
    final String TAG = this.getClass().getSimpleName();
    LinearLayout home_ly;
    BottomNavigationView bottomNavigationView;

    HomeFragment homeFragment;
    MapFragment mapFragment;
    AskFragment askFragment;

//    ImageButton prev;
//    ImageButton next;
//    ViewFlipper flipper;
//
//    ImageView img1;
//    ImageView img2;
//    ImageView img3;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        init();
        SettingListener();

//        Button executeUnity = (Button)findViewById(R.id.executeUnity);
//        executeUnity.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View v) {
//                Intent intent = new Intent(getApplicationContext(), UnityPlayerActivity.class);
//                startActivity(intent);
//            }
//        });
        homeFragment = new HomeFragment(); //홈화면에 대한 프래그먼트 생성
        mapFragment = new MapFragment(); //맵 선택 화면에 대한 프래그먼트 생성
        askFragment = new AskFragment(); //Ask 화면에 대한 프래그먼트 생성


        //제일 처음 띄워줄 뷰를 세팅 --> 맨 처음에 로딩 후 home 화면이 뜨게 됨
        getSupportFragmentManager().beginTransaction().replace(R.id.home_ly,homeFragment).commit();

        //bottomNavigationView.setSelectedItemId(R.id.home);

//        flipper = (ViewFlipper) findViewById(R.id.flipper);
//        prev = (ImageButton) findViewById(R.id.prev);
//        next = (ImageButton) findViewById(R.id.next);
//
//        prev.setOnClickListener(this);
//        next.setOnClickListener(this);
//
//        img1 = (ImageView) findViewById(R.id.img1);
//        img2 = (ImageView) findViewById(R.id.img2);
//        img3 = (ImageView) findViewById(R.id.img3);


    }

    private void init() {
        home_ly = findViewById(R.id.home_ly);
        bottomNavigationView = findViewById(R.id.bottomNavigationView);
    }

    private void SettingListener() {
        bottomNavigationView.setOnNavigationItemSelectedListener(new TabSelectedListener());
    }

    class TabSelectedListener implements BottomNavigationView.OnNavigationItemSelectedListener {
        @Override
        public boolean onNavigationItemSelected(@NonNull MenuItem menuItem) { //하단 네비게이션바 리스너
            switch (menuItem.getItemId()) {
                case R.id.tab_home: { //하단바에서 홈 아이콘 클릭시
                    onFragmentChange(0);
                    return true;
                }

                case R.id.tab_map: { //하단바에서 맵 아이콘 클릭시
                    onFragmentChange(1);
                    return true;
                }

                case R.id.tab_ask: { //하단바에서 Ask 아이콘 클릭시
                    onFragmentChange(2);
                    return true;
                }
            }
            return false;
        }

    }

    //프래그먼트는 프래그먼트끼리 직접 소통 불가능, 프래그먼트는 액티비티를 통하여 소통
    public void onFragmentChange(int index){
        if(index == 0 ){
            getSupportFragmentManager().beginTransaction().replace(R.id.home_ly,  homeFragment).commit(); //home 프래그먼트로 변경
        }else if(index == 1){
            getSupportFragmentManager().beginTransaction().replace(R.id.home_ly, mapFragment).commit(); //map 프래그먼트로 변경
        }else if(index == 2){
            getSupportFragmentManager().beginTransaction().replace(R.id.home_ly, askFragment).commit(); //ask 프래그먼트로 변경
        }
    }

//    //ViewFlipper 클릭 이벤트 핸들러
//    public void onClick(View v) {
//        if(v == prev) {
//            flipper.showPrevious();
//        }
//        else if(v == next) {
//            flipper.showNext();
//        }
//    }
}